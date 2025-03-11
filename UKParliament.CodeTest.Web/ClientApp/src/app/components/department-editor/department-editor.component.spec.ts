import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';  
import { DepartmentEditorComponent } from './department-editor.component';
import { DepartmentService } from '../../services/department.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';

describe('DepartmentEditorComponent', () => {
  let component: DepartmentEditorComponent;
  let fixture: ComponentFixture<DepartmentEditorComponent>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getById', 'create', 'update']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [DepartmentEditorComponent],
      imports: [FormsModule, HttpClientTestingModule],
      providers: [
        { provide: DepartmentService, useValue: mockDepartmentService }, // Mock Service
        { provide: Router, useValue: mockRouter }, // Mock Router
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } } // Mock Route ID
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DepartmentEditorComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load department on init if ID is present', () => {
    const mockDepartment = { id: 1, name: 'HR' };
    mockDepartmentService.getById.and.returnValue(of(mockDepartment)); // Mock Service Response

    component.ngOnInit();
    fixture.detectChanges();

    expect(mockDepartmentService.getById).toHaveBeenCalledWith(1);
    expect(component.department.name).toBe('HR');
  });

  it('should handle null or undefined department response', () => {
    mockDepartmentService.getById.and.returnValue(of({ id: 0, name: '' }));

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.department).toEqual({ id: 0, name: '' });
  });

  it('should call update if department has an ID', () => {
    component.department = { id: 1, name: 'Finance' };
    mockDepartmentService.update.and.returnValue(of(void 0));

    component.saveDepartment();

    expect(mockDepartmentService.update).toHaveBeenCalledWith(component.department);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/departments']);
  });

  it('should call create if department has no ID', () => {
    component.department = { id: 0, name: 'IT' };
    mockDepartmentService.create.and.returnValue(of({ id: 1, name: 'IT' }));

    component.saveDepartment();

    expect(mockDepartmentService.create).toHaveBeenCalledWith(component.department);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/departments']);
  });

  it('should navigate back when cancel is clicked', () => {
    component.cancel();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/departments']);
  });
});
