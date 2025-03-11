import { importProvidersFrom, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PersonEditorComponent } from './components/person-editor/person-editor.component';
import { HomeComponent } from './components/home/home.component';
import { PersonListComponent } from './components/person-list/person-list.component';
import { DepartmentListComponent } from './components/department-list/department-list.component';
import { DepartmentEditorComponent } from './components/department-editor/department-editor.component';



const routes: Routes = [
  { path: '', component: HomeComponent }, // Default route
  { path: 'persons', component: PersonListComponent }, // List of persons
  { path: 'persons/edit/:id', component: PersonEditorComponent }, // Edit Person
  { path: 'persons/new', component: PersonEditorComponent }, // Add New Person
  { path: 'departments', component: DepartmentListComponent }, // List of Departments
  { path: 'departments/edit/:id', component: DepartmentEditorComponent }, // Edit Department
  { path: 'departments/new', component: DepartmentEditorComponent } // Add Department

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
