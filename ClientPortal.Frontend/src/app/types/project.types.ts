export interface ProjectDto {
  id: string;
  name: string;
  description: string;
  scopeFeatures: number;
  currentFeaturesCount: number;
  completedFeaturesCount: number;
}

export interface CreateProjectDto {
  name: string;
  description: string;
  scopeFeatures: number;
}
