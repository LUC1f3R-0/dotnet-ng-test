import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { About } from './pages/about/about';
import { Contact } from './pages/contact/contact';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { authGuard } from './guards/auth.guard';
import { guestGuard } from './guards/guest.guard';

export const routes: Routes = [
  { path: '', component: Home, },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'register', component: Register, canActivate: [guestGuard] },

  {
    path: 'user',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/user/admin/admin').then(m => m.Admin),
      // canActivate:
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/user/tenant/tenant').then(m => m.Tenant)
  },
  // {loadCh}
];
