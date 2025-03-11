import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PersonEditorComponent } from './person-editor.component';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let mockPersonService: jasmine.SpyObj<PersonService>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockPersonService = jasmine.createSpyObj('PersonService', ['getById', 'create', 'update']);
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getDepartments']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    mockDepartmentService.getDepartments.and.returnValue(of([{ id: 1, name: 'HR' }]));

    mockPersonService.getById.and.returnValue(of({
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@example.com',
      dateOfBirth: '1990-01-01',
      departmentId: 1,
      department: { id: 1, name: 'HR' }
    }));

    await TestBed.configureTestingModule({
      declarations: [PersonEditorComponent],
      imports: [FormsModule, HttpClientTestingModule],
      providers: [
        { provide: PersonService, useValue: mockPersonService },
        { provide: DepartmentService, useValue: mockDepartmentService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load departments on init', () => {
    component.ngOnInit();
    fixture.detectChanges();

    expect(mockDepartmentService.getDepartments).toHaveBeenCalled();
    expect(component.departments.length).toBe(1);
    expect(component.departments[0].name).toBe('HR');
  });

  it('should load person on init if ID is present', () => {
    component.ngOnInit();
    fixture.detectChanges();

    expect(mockPersonService.getById).toHaveBeenCalledWith(1);
    expect(component.person?.firstName).toBe('John');
  });

  it('should call update when person has an ID', () => {
    component.person = { id: 1, firstName: 'Jane', lastName: 'Doe', email: 'jane@example.com', dateOfBirth: '1985-05-20', departmentId: 2, department: { id: 2, name: 'Finance' } };
    mockPersonService.update.and.returnValue(of(void 0));

    component.savePerson({ valid: true } as any);

    expect(mockPersonService.update).toHaveBeenCalledWith(component.person);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/persons']);
  });

  it('should call create when person has no ID', () => {
    component.person = { id: 0, firstName: 'New', lastName: 'Person', email: 'new@example.com', dateOfBirth: '2000-01-01', departmentId: 3, department: { id: 3, name: 'IT' } };
    const { id, ...personWithoutId } = component.person;
    mockPersonService.create.and.returnValue(of({ id: 1, ...personWithoutId }));

    component.savePerson({ valid: true } as any);

    expect(mockPersonService.create).toHaveBeenCalledWith(component.person);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/persons']);
  });

  it('should NOT call create/update when form is invalid', () => {
    component.savePerson({ valid: false } as any);

    expect(mockPersonService.create).not.toHaveBeenCalled();
    expect(mockPersonService.update).not.toHaveBeenCalled();
  });

  it('should navigate to /persons when cancel is clicked', () => {
    component.cancel();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/persons']);
  });
});
