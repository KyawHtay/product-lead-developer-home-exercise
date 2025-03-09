import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private baseUrl = environment.apiUrl+"/api/person";
  constructor(private http: HttpClient) { }

  getById(id: number): Observable<PersonViewModel> {
    console.log(this.baseUrl + `/${id}`);
    return this.http.get<PersonViewModel>(this.baseUrl + `/${id}`)
  }

  getAll(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(this.baseUrl);
  }
  create(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<PersonViewModel>(this.baseUrl, person);
  }

  update(person: PersonViewModel): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${person.id}`, {
      id: person.id,
      firstName: person.firstName,
      lastName: person.lastName,
      email: person.email,
      dateOfBirth: person.dateOfBirth,
      departmentId: person.departmentId
    });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
