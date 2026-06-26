import { Component, OnInit, signal, inject } from '@angular/core';
import { ProjectService} from '../services/project.service';
import { FeatureService } from '../services/feature.service';
import { ProjectDto } from '../types/project.types';
import { FeatureDto } from '../types/feature.types';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-project-detail',
  imports: [RouterLink],
  templateUrl: './project-detail.html',
  styleUrl: './project-detail.css',
})
export class ProjectDetail implements OnInit {
  private projectService = inject(ProjectService);
  private featureService = inject(FeatureService);
  private activatedRoute = inject(ActivatedRoute);

  project = signal<ProjectDto | null>(null);
  features = signal<FeatureDto[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe({
      next: (params) => {
        const projectId = params.get('projectId');

        if (!projectId) {
          this.error.set('Project id is missing');
          this.loading.set(false);
          return;
        }

        this.loading.set(true);

        forkJoin({
          projectData: this.projectService.getProjectById(projectId),
          featuresData: this.featureService.getFeaturesByProjectId(projectId),
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
}
