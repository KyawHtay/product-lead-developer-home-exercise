import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AppRoutingModule } from './app-routing.module';

import { PersonEditorComponent } from './components/person-editor/person-editor.component';
import { importProvidersFrom } from '@angular/core';
import { PersonListComponent } from './components/person-list/person-list.component';
import { DepartmentEditorComponent } from './components/department-editor/department-editor.component';
import { DepartmentListComponent } from './components/department-list/department-list.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';


@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        PersonListComponent,
        PersonEditorComponent,
        DepartmentListComponent,
        DepartmentEditorComponent
    ],
    bootstrap: [AppComponent],
    imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    AppRoutingModule,
    MatToolbarModule,
    MatButtonModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule

],
    providers: [
        provideHttpClient(withInterceptorsFromDi()),
    ]
})
export class AppModule { }
