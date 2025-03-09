import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';

import { PersonEditorComponent } from './person-editor.component';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentVewModel } from '../../models/department-view-model';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let mockPersonService: jasmine.SpyObj<PersonService>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;

  beforeEach(async () => {
    // ✅ Create service spies
    mockPersonService = jasmine.createSpyObj('PersonService', ['create', 'update']);
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getDepartments']);

    await TestBed.configureTestingModule({
      declarations: [PersonEditorComponent],
      imports: [FormsModule, HttpClientTestingModule], 
      providers: [
        { provide: PersonService, useValue: mockPersonService },
        { provide: DepartmentService, useValue: mockDepartmentService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;

    // ✅ Mock the departments list to prevent 'undefined' error
    const mockDepartments: DepartmentVewModel[] = [{ id: 1, name: 'HR' }, { id: 2, name: 'Finance' }];
    mockDepartmentService.getDepartments.and.returnValue(of(mockDepartments));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load departments on init', () => {
    expect(mockDepartmentService.getDepartments).toHaveBeenCalled();
    expect(component.departments.length).toBe(2);
  });

  it('should call update when person has an ID', () => {
    const mockPerson: PersonViewModel = { id: 1, firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01', departmentId: 1, department: { id: 1, name: 'HR' } };
    component.person = mockPerson;

    mockPersonService.update.and.returnValue(of(undefined));

    const fakeForm = { valid: true } as NgForm;
    component.savePerson(fakeForm);

    expect(mockPersonService.update).toHaveBeenCalledWith(mockPerson);
  });

  it('should call create when person has no ID', () => {
    const mockPerson: PersonViewModel = { id: 0, firstName: 'Alice', lastName: 'Smith', email: 'alice@example.com', dateOfBirth: '1995-06-15', departmentId: 2, department: { id: 2, name: 'Finance' } };
    component.person = mockPerson;

    mockPersonService.create.and.returnValue(of(mockPerson));

    const fakeForm = { valid: true } as NgForm;
    component.savePerson(fakeForm);

    expect(mockPersonService.create).toHaveBeenCalledWith(mockPerson);
  });

  it('should NOT call create/update when form is invalid', () => {
    const fakeForm = { valid: false } as NgForm;
    component.savePerson(fakeForm);

    expect(mockPersonService.create).not.toHaveBeenCalled();
    expect(mockPersonService.update).not.toHaveBeenCalled();
  });

  it('should emit close event on cancel', () => {
    spyOn(component.close, 'emit');

    component.cancel();

    expect(component.close.emit).toHaveBeenCalled();
  });
});
