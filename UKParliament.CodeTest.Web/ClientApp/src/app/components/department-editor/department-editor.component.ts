import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from '../../services/department.service';
import { DepartmentVewModel } from '../../models/department-view-model';


@Component({
  selector: 'app-department-editor',
  templateUrl: './department-editor.component.html',
  styleUrls: ['./department-editor.component.scss'],
})
export class DepartmentEditorComponent implements OnInit {
  department: DepartmentVewModel = { id: 0, name: '' };

  constructor(
    private departmentService: DepartmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
   
    if (id) {
      this.departmentService.getById(id)?.subscribe(dept => {
        this.department = dept ?? { id: 0, name: '' };
      }, () => {
        this.department = { id: 0, name: '' };
      });
    }
  }

  saveDepartment(): void {
    if (this.department.id) {
      this.departmentService.update(this.department).subscribe(() => this.router.navigate(['/departments']));
    } else {
      this.departmentService.create(this.department).subscribe(() => this.router.navigate(['/departments']));
    }
  }

  cancel(): void {
    this.router.navigate(['/departments']);
  }
}
