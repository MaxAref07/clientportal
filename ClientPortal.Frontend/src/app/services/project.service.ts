import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProjectDto } from '../types/project.types';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private httpClient = inject(HttpClient);

  private apiUrl = '/Project';

  getAllProjects(): Observable<ProjectDto[]> {
    return this.httpClient.get<ProjectDto[]>(`${this.apiUrl}`, {});
  }
}
