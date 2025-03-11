import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DepartmentService } from '../../services/department.service';
import { DepartmentVewModel } from '../../models/department-view-model';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss']
})
export class DepartmentListComponent implements OnInit {
  departments: DepartmentVewModel[] = [];

  constructor(private departmentService: DepartmentService, private router: Router) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(data => this.departments = data);
  }

  editDepartment(id: number): void {
    this.router.navigate(['/departments/edit', id]);
  }

  addDepartment(): void {
    this.router.navigate(['/departments/new']);
  }

  deleteDepartment(id: number): void {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.delete(id).subscribe(() => {
        this.departments = this.departments.filter(dept => dept.id !== id);
      });
    }
  }
}
