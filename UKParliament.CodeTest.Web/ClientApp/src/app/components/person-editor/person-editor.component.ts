import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PersonViewModel } from '../../models/person-view-model';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { DepartmentVewModel } from '../../models/department-view-model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit {
  @Input() person: PersonViewModel | null = null;
  @Output() close = new EventEmitter<void>();

  departments: DepartmentVewModel[] = [];
  today: string = new Date().toISOString().split('T')[0];
  isNavigated: boolean = false; // Track if opened via route

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDepartments();

    // If no person is passed as @Input(), check for an ID in the route
    if (!this.person) {
      const id = Number(this.route.snapshot.paramMap.get('id'));
      if (id) {
        this.isNavigated = true;
        this.loadPerson(id);
      } else {
        this.person = {
          id: 0,
          firstName: '',
          lastName: '',
          dateOfBirth: '',
          email: '',
          departmentId: 0,
          department: undefined
        };
      }
    }
  }

  loadPerson(id: number): void {
    this.personService.getById(id).subscribe(person => {
      this.person = person;
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(depts => this.departments = depts);
  }

  savePerson(form: NgForm): void {
    if (!form.valid || !this.person) return;

    if (this.person.id) {
      this.personService.update(this.person).subscribe(() => this.handleClose());
    } else {
      this.personService.create(this.person).subscribe(() => this.handleClose());
    }
  }

  cancel(): void {
    this.handleClose();
  }
  private handleClose(): void {
    if (this.isNavigated) {
      this.router.navigate(['/persons']); // ✅ If in route-based edit, navigate back
    } else {
      this.close.emit(); // ✅ Otherwise, close the editor inside the list
    }
  }
}
