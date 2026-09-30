import { Routes } from '@angular/router';
import { Home } from './home/home';
import { About } from './about/about';
import { Contact } from './contact/contact';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  {
    path: 'user',
    loadComponent: () => import('./user/admin/admin').then(m => m.Admin),
      // canActivate:
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./user/dashboard/dashboard').then(m => m.Dashboard)
  },
  // {loadCh}
];
