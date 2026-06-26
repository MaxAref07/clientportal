import { Routes } from '@angular/router';
import { Home } from './home/home'
import { ProjectDetail } from './project-detail/project-detail';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'projects/:projectId',
    component: ProjectDetail,
  },
];
