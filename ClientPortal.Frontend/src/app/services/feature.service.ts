import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeatureDto } from '../types/feature.types';

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
}
