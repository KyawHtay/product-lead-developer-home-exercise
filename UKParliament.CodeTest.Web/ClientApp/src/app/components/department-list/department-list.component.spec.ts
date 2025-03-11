import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DepartmentListComponent } from './department-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DepartmentService } from '../../services/department.service';

describe('DepartmentListComponent', () => {
  let component: DepartmentListComponent;
  let fixture: ComponentFixture<DepartmentListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DepartmentListComponent],
      imports: [HttpClientTestingModule],
      providers: [DepartmentService]
    }).compileComponents();

    fixture = TestBed.createComponent(DepartmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
