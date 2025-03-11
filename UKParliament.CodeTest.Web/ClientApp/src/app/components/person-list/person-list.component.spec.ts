import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonListComponent } from './person-list.component';
import { PersonService } from '../../services/person.service';
import { of } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('PersonListComponent', () => {
  let component: PersonListComponent;
  let fixture: ComponentFixture<PersonListComponent>;
  let mockPersonService: jasmine.SpyObj<PersonService>;

  beforeEach(async () => {
    mockPersonService = jasmine.createSpyObj('PersonService', ['getAll', 'delete']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [PersonListComponent],
      providers: [{ provide: PersonService, useValue: mockPersonService }]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PersonListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load people on init', () => {
    const mockPeople = [
      { id: 1, firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01', departmentId: 1, department: { id: 1, name: 'HR' } }
    ];
    mockPersonService.getAll.and.returnValue(of(mockPeople));

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.people.length).toBe(1);
    expect(component.people[0].firstName).toBe('John');
  });

  it('should delete a person', () => {
    const mockPeople = [
      { id: 1, firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01', departmentId: 1, department: { id: 1, name: 'HR' } }
    ];
    component.people = mockPeople;
    mockPersonService.delete.and.returnValue(of(void 0));

    component.deletePerson(1);

    expect(component.people.length).toBe(0);
  });
});
