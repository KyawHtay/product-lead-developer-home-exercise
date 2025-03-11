import { TestBed } from '@angular/core/testing';
import { DepartmentService } from './department.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DepartmentVewModel } from '../models/department-view-model';

describe('DepartmentService', () => {
  let service: DepartmentService;
  let httpMock: HttpTestingController;
  const apiUrl = 'https://localhost:7048/api/departments';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DepartmentService]
    });

    service = TestBed.inject(DepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch departments', () => {
    const mockDepartments: DepartmentVewModel[] = [{ id: 1, name: 'HR' }];

    service.getDepartments().subscribe(departments => {
      expect(departments.length).toBe(1);
      expect(departments[0].name).toBe('HR');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockDepartments);
  });
});
