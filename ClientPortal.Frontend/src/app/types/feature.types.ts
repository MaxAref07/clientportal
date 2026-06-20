export enum FeaturePriority {
  Low = 0,
  Medium = 1,
  High = 2,
}

export enum FeatureStatus {
  ToDo = 0,
  InProgress = 1,
  OnReview = 2,
  Done = 3,
}

export interface FeatureDto {
  id: string;
  name: string;
  priority: FeaturePriority;
  status: FeatureStatus;
  description: string;
  projectId: string;
}
