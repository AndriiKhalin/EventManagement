import { Routes } from '@angular/router';
import { AuthGuard } from '../app/core/guards/auth-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/events',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        loadComponent: () => import('./components/auth/login/login').then(m => m.LoginComponent)
      },
      {
        path: 'register',
        loadComponent: () => import('./components/auth/register/register').then(m => m.RegisterComponent)
      }
    ]
  },
  {
    path: 'events',
    children: [
      {
        path: '',
        loadComponent: () => import('./components/events/event-list/event-list').then(m => m.EventListComponent)
      },
      {
        path: 'create',
        canActivate: [AuthGuard],
        loadComponent: () => import('./components/events/create-event/create-event').then(m => m.CreateEventComponent)
      },
      {
        path: ':id',
        loadComponent: () => import('./components/events/event-details/event-details').then(m => m.EventDetailsComponent)
      },
      {
        path: ':id/edit',
        canActivate: [AuthGuard],
        loadComponent: () => import('./components/events/edit-event/edit-event').then(m => m.EditEventComponent)
      }
    ]
  },
  {
    path: 'my-events',
    canActivate: [AuthGuard],
    loadComponent: () => import('./components/calendar/my-events-calendar/my-events-calendar').then(m => m.MyEventsCalendarComponent)
  },
  {
    path: '**',
    redirectTo: '/events'
  }
];
