import { GuardService } from './guard.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToastyModule } from 'ngx-toasty';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FooterComponent } from './footer/footer.component';
import { NavigationComponent } from './navigation/navigation.component';
import { RegisterComponent } from './register/register.component';

import { UserService } from './user.service';
import { AppErrorHandler } from './app.error-handler';
import { LoginComponent } from './login/login.component';
import { ConfigService } from './config.service';
import { SpinnerComponent } from './spinner/spinner.component';
import { SpinnerService } from './spinner.service';
import { NotesComponent } from './notes/notes.component';
import { AdminComponent } from './admin/admin.component';
import { AdminGuardService } from './admin.guard.service';
import { CreateNoteComponent } from './create-note/create-note.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FooterComponent,
    NavigationComponent,
    RegisterComponent,
    LoginComponent,
    SpinnerComponent,
    NotesComponent,
    AdminComponent,
    CreateNoteComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    ToastyModule.forRoot(),
    ReactiveFormsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'notes', component: NotesComponent, canActivate: [GuardService] },
      { path: 'admin', component: AdminComponent, canActivate: [AdminGuardService] },
    ])
  ],
  providers: [
    AdminGuardService,
    GuardService,
    SpinnerService,
    UserService,
    { provide: ErrorHandler, useClass: AppErrorHandler },
    ConfigService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
