import { TestBed } from '@angular/core/testing';
import { PersonService } from './person.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonViewModel } from '../models/person-view-model';
import {environment} from '../../environments/environment';
describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const apiUrl = environment.apiUrl + '/api/person';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PersonService]
    });

    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensures no pending HTTP requests
  });

  it('should retrieve all persons', () => {
    const mockPeople: PersonViewModel[] = [
      { id: 1, firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01', departmentId: 1, department: { id: 1, name: 'HR' } }
    ];

    service.getAll().subscribe(people => {
      expect(people.length).toBe(1);
      expect(people[0].firstName).toBe('John');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPeople);
  });

  it('should create a new person', () => {
    const newPerson: PersonViewModel = { id: 0, firstName: 'Alice', lastName: 'Smith', email: 'alice@example.com', dateOfBirth: '1995-06-15', departmentId: 2, department: { id: 2, name: 'Finance' } };

    service.create(newPerson).subscribe(person => {
      expect(person.firstName).toBe('Alice');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(newPerson);
  });

  it('should delete a person', () => {
    const personId = 1;

    service.delete(personId).subscribe(response => {
      expect(response).toBeNull(); // Expect `null` instead of `undefined`
    });

    const req = httpMock.expectOne(`${apiUrl}/${personId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null); //Keep flushing `null`
  });

});
