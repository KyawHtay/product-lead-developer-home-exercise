import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentVewModel } from '../../models/department-view-model';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit {
  person: PersonViewModel = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateOfBirth: '',
    departmentId: 0,
    department: undefined
  };
  departments: DepartmentVewModel[] = [];

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDepartments();

    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.personService.getById(id).subscribe(person => this.person = person);
    }
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(depts => this.departments = depts);
  }

  savePerson(): void {
    console.log(this.person);
    if (this.person.id) {
      this.personService.update(this.person).subscribe(() => this.router.navigate(['/persons']));
    } else {
      this.personService.create(this.person).subscribe(() => this.router.navigate(['/persons']));
    }
  }

  cancel(): void {
    this.router.navigate(['/persons']);
  }
}
