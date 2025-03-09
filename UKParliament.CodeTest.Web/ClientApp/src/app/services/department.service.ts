import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DepartmentVewModel } from '../models/department-view-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private apiUrl = environment.apiUrl + '/api/departments';

  constructor(private http: HttpClient) {}

  getDepartments(): Observable<DepartmentVewModel[]> {
    return this.http.get<DepartmentVewModel[]>(this.apiUrl);
  }

  getById(id: number): Observable<DepartmentVewModel> {
    return this.http.get<DepartmentVewModel>(`${this.apiUrl}/${id}`);
  }

  create(department: DepartmentVewModel): Observable<DepartmentVewModel> {
    return this.http.post<DepartmentVewModel>(this.apiUrl, department);
  }

  update(department: DepartmentVewModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${department.id}`, department);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
