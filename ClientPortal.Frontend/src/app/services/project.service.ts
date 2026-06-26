import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateProjectDto, ProjectDto } from '../types/project.types';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private httpClient = inject(HttpClient);

  private apiUrl = '/Project';

  getAllProjects(): Observable<ProjectDto[]> {
    return this.httpClient.get<ProjectDto[]>(`${this.apiUrl}`, {});
  }

  getProjectById(projectId: string): Observable<ProjectDto> {
    return this.httpClient.get<ProjectDto>(`${this.apiUrl}/${projectId}`);
  }

  createProject(createProjectDto: CreateProjectDto): Observable<ProjectDto> {
    return this.httpClient.post<ProjectDto>(`${this.apiUrl}`, createProjectDto);
  }
}
