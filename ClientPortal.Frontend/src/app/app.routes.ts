import { Routes } from '@angular/router';
import { Home } from './home/home'
import { ProjectDetail } from './project-detail/project-detail';
import { Login } from './login/login';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: Login,
  },
  {
    path: '',
    component: Home,
    canActivate: [authGuard],
  },
  {
    path: 'projects/:projectId',
    component: ProjectDetail,
    canActivate: [authGuard],
  },
];
