import { Component, OnInit, inject, signal } from '@angular/core';
import { ProjectService } from '../services/project.service';
import { ProjectDto } from '../types/project.types';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private projectService = inject(ProjectService);

  projects = signal<ProjectDto[]>([])

  ngOnInit() {
    this.projectService.getAllProjects().subscribe({
      next: (data) => {
        this.projects.set(data);
      },
      error: err => {
        console.error("Error getting projects", err);
      }
    });
  }
}
