import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AppRoutingModule } from './app-routing.module';

import { PersonEditorComponent } from './components/person-editor/person-editor.component';
import { NavbarComponent } from "./components/navbar/navbar.component";
import { importProvidersFrom } from '@angular/core';
import { PersonListComponent } from './components/person-list/person-list.component';
import { DepartmentEditorComponent } from './components/department-editor/department-editor.component';
import { DepartmentListComponent } from './components/department-list/department-list.component';


@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        PersonListComponent,
        PersonEditorComponent,
        DepartmentListComponent,
        DepartmentEditorComponent,
        NavbarComponent
    ],
    bootstrap: [AppComponent],
    imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    AppRoutingModule,

],
    providers: [
        provideHttpClient(withInterceptorsFromDi()),
        importProvidersFrom(NavbarComponent)
    ]
})
export class AppModule { }
