import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateFeatureDto, FeatureDto } from '../types/feature.types';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {
  private httpClient = inject(HttpClient);

  private readonly apiUrl = '/Feature';

  getFeaturesByProjectId(projectId: string): Observable<FeatureDto[]> {
    return this.httpClient.get<FeatureDto[]>(this.apiUrl, {
      params: { projectId },
    });
  }

  createFeature(createFeatureDto: CreateFeatureDto): Observable<FeatureDto> {
    return this.httpClient.post<FeatureDto>(`${this.apiUrl}`, createFeatureDto);
  }
}
