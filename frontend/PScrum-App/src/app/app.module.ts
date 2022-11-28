import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";

import {AutoCompleteModule} from 'primeng/autocomplete';
import {TabViewModule} from 'primeng/tabview';

import { AppRoutingModule } from '@app/app-routing.module';
import { AppComponent } from '@app/app.component';
import { NavComponent } from '@app/shared/nav/nav.component';
import { TeamPerformanceComponent } from '@app/components/team-performance/team-performance.component';
import { TeamPerformanceListComponent } from '@app/components/team-performance/team-performance-list/team-performance-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JiraService } from '@app/services/jira.service';
import { DateTimeFormatPipe } from '@app/helpers/dateTimeFormat.pipe';
import { TitleComponent } from '@app/shared/title/title.component';
import { DashboardComponent } from '@app/components/dashboard/dashboard.component';
import { ProfileComponent } from '@app/components/user/profile/profile.component';
import { ContactComponent } from '@app/components/contact/contact.component';
import { UserComponent } from '@app/components/user/user.component';
import { LoginComponent } from '@app/components/user/login/login.component';
import { RegistrationComponent } from '@app/components/user/registration/registration.component';
import { AccountService } from '@app/services/account.service';
import { JwtInterceptor } from '@app/interceptors/jwt.Interceptor';
import { HomeComponent } from '@app/components/home/home.component';

import { SelectDropDownModule } from 'ngx-select-dropdown'
import {DropdownModule} from 'primeng/dropdown';


import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule }  from '@angular/material/select';
import { ProjectComponent } from './components/project/project.component';
import { ProjectListComponent } from './components/project/project-list/project-list.component';
import { ProjectSyncComponent } from './components/project/project-sync/project-sync.component';
import { DomainComponent } from './components/domain/domain.component';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
registerLocaleData(ptBr);


@NgModule({
  declarations: [
    AppComponent,
    TeamPerformanceComponent,
    NavComponent,
    DateTimeFormatPipe,
    TitleComponent,
    DashboardComponent,
    ProfileComponent,
    ContactComponent,
    TeamPerformanceListComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent,
    ProjectComponent,
    ProjectListComponent,
    ProjectSyncComponent,
    DomainComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule,
    TooltipModule,
    ModalModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    MatTabsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatSelectModule,
    SelectDropDownModule,
    ProgressbarModule,
    AutoCompleteModule,
    TabViewModule,
    DropdownModule
  ],
  providers: [
    JiraService,
    BsModalService,
    AccountService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: LOCALE_ID, useValue: 'pt' }
  ],
  bootstrap: [AppComponent],

})
export class AppModule { }
