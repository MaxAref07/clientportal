import { Component, OnInit, signal, inject } from '@angular/core';
import { ProjectService} from '../services/project.service';
import { FeatureService } from '../services/feature.service';
import { ProjectDto } from '../types/project.types';
import { CreateFeatureDto, FeatureDto, FeaturePriority } from '../types/feature.types';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-project-detail',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './project-detail.html',
  styleUrl: './project-detail.css',
})
export class ProjectDetail implements OnInit {
  private projectService = inject(ProjectService);
  private featureService = inject(FeatureService);
  private activatedRoute = inject(ActivatedRoute);
  private formBuilder = inject(FormBuilder);
  private projectId: string | null | undefined;
  project = signal<ProjectDto | null>(null);
  features = signal<FeatureDto[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  isFormOpen = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);
  submitError = signal<string | null>(null);

  createFeatureForm = this.formBuilder.group({
    name: this.formBuilder.control('', { validators: [Validators.required], nonNullable: true }),
    description: this.formBuilder.control('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    priority: this.formBuilder.control<FeaturePriority>(FeaturePriority.Low, {
      validators: [Validators.required],
      nonNullable: true,
    }),
  });

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe({
      next: (params) => {
        this.projectId = params.get('projectId');

        if (!this.projectId) {
          this.error.set('Project id is missing');
          this.loading.set(false);
          return;
        }

        this.loading.set(true);

        forkJoin({
          projectData: this.projectService.getProjectById(this.projectId),
          featuresData: this.featureService.getFeaturesByProjectId(this.projectId),
        }).subscribe({
          next: (data) => {
            this.project.set(data.projectData);
            this.features.set(data.featuresData);
            this.loading.set(false);
          },
          error: () => {
            this.error.set('Could not load project data');
            this.loading.set(false);
          },
        });
      },
    });
  }

  toggleForm() {
    this.isFormOpen.update((x) => !x);
    if (!this.isFormOpen()) {
      this.resetFormState();
    }
  }

  resetFormState() {
    this.createFeatureForm.reset({ name: '', description: '', priority: FeaturePriority.Low });
    this.submitError.set(null);
  }

  onSubmit() {
    if (this.createFeatureForm.invalid) {
      this.createFeatureForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    const formRaw = this.createFeatureForm.getRawValue();

    const newFeature: CreateFeatureDto = {
      name: formRaw.name,
      description: formRaw.description,
      priority: formRaw.priority,
      projectId: this.projectId!,
    };

    this.featureService.createFeature(newFeature).subscribe({
      next: (createdFeature) => {
        this.features.update((list) => [createdFeature, ...list]);

        this.isFormOpen.set(false);
        this.isSubmitting.set(false);
        this.resetFormState();
      },
      error: () => {
        this.submitError.set('Failed to create feature. Please try again.');
        this.isSubmitting.set(false);
      }
    })
  }
}
