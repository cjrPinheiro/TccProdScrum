import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactComponent } from '@app/components/contact/contact.component';
import { DashboardComponent } from '@app/components/dashboard/dashboard.component';
import { ProfileComponent } from '@app/components/user/profile/profile.component';
import { LoginComponent } from '@app/components/user/login/login.component';
import { RegistrationComponent } from '@app/components/user/registration/registration.component';
import { UserComponent } from '@app/components/user/user.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from '@app/components/home/home.component';
import { TeamPerformanceComponent } from './components/team-performance/team-performance.component';
import { TeamPerformanceListComponent } from './components/team-performance/team-performance-list/team-performance-list.component';
import { ProjectComponent } from './components/project/project.component';
import { ProjectListComponent } from './components/project/project-list/project-list.component';
import { ProjectSyncComponent } from './components/project/project-sync/project-sync.component';
import { DomainComponent } from './components/domain/domain.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children:[
      { path: 'performance', redirectTo: 'performance/list'},
      {
        path: 'performance', component: TeamPerformanceComponent,
        children: [
          { path: 'list', component: TeamPerformanceListComponent }
        ]
      },
      { path: 'user/profile', component: ProfileComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'domain', component: DomainComponent },
      { path: 'projects', redirectTo: 'projects/list'},

      {
        path: 'projects', component: ProjectComponent,
        children: [
          { path: 'list', component: ProjectListComponent },
          { path: 'sync', component: ProjectSyncComponent }
        ]
      },
    ],
  },
  { path:'user', redirectTo: 'user/profile' },
  { path:'user', component: UserComponent,
  children:[
    { path: 'login', component: LoginComponent },
    { path: 'registration', component: RegistrationComponent }
  ]},
  { path: 'home', component: HomeComponent },
  { path: '**', redirectTo: 'dashboard', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
