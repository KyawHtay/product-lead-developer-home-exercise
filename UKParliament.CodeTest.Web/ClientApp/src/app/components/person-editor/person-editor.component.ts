import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PersonViewModel } from '../../models/person-view-model';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { DepartmentVewModel } from '../../models/department-view-model';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit {
  @Input() person: PersonViewModel = {
    id: 0,
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    email: '',
    departmentId: 0,
    department: undefined
  }; // Person to edit

  @Output() close = new EventEmitter<void>(); // Emit event to close editor

  departments: DepartmentVewModel[] = []; // ✅ Store department list
  today: string = new Date().toISOString().split('T')[0]; // Get today's date in YYYY-MM-DD format

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(depts => this.departments = depts);
  }

  savePerson(form: NgForm): void {
    if (!form.valid) return; // Prevent submission if form is invalid

    if (this.person.id) {
      this.personService.update(this.person).subscribe(() => this.close.emit());
    } else {
      this.personService.create(this.person).subscribe(() => this.close.emit());
    }
  }

  cancel(): void {
    this.close.emit();
  }
}
