import { Component, OnInit, inject, signal } from '@angular/core';
import { ProjectService } from '../services/project.service';
import { AuthService } from '../services/auth.service';
import { CreateProjectDto, ProjectDto } from '../types/project.types';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [RouterModule, ReactiveFormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private projectService = inject(ProjectService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private formBuilder = inject(FormBuilder);

  projects = signal<ProjectDto[]>([]);
  isFormOpen = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);
  submitError = signal<string | null>(null);

  createProjectForm = this.formBuilder.group({
    name: this.formBuilder.control('', { validators: [Validators.required], nonNullable: true }),
    description: this.formBuilder.control('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    scopeFeatures: this.formBuilder.control(1, {
      validators: [Validators.required, Validators.min(1)],
      nonNullable: true,
    }),
  });

  ngOnInit() {
    this.projectService.getAllProjects().subscribe({
      next: (data) => this.projects.set(data),
      error: (err) => console.error('Error getting projects', err),
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  toggleForm() {
    this.isFormOpen.update((x) => !x);
    if (!this.isFormOpen()) {
      this.resetFormState();
    }
  }

  resetFormState() {
    this.createProjectForm.reset({ name: '', description: '', scopeFeatures: 1 });
    this.submitError.set(null);
  }

  onSubmit() {
    if (this.createProjectForm.invalid) {
      this.createProjectForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    const formRaw = this.createProjectForm.getRawValue();

    const newProject: CreateProjectDto = {
      name: formRaw.name,
      description: formRaw.description,
      scopeFeatures: formRaw.scopeFeatures,
    };

    this.projectService.createProject(newProject).subscribe({
      next: (createdProject) => {
        this.projects.update((list) => [createdProject, ...list]);

        this.isFormOpen.set(false);
        this.isSubmitting.set(false);
        this.resetFormState();
      },
      error: (err) => {
        this.submitError.set('Failed to create project. Please try again.');
        this.isSubmitting.set(false);
      },
    });
  }
}
