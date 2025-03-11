import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PersonViewModel } from '../../models/person-view-model';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { DepartmentVewModel } from '../../models/department-view-model';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit {
  @Input() person!: PersonViewModel;
  @Output() close = new EventEmitter<void>();

  departments: DepartmentVewModel[] = [];
  today: string = new Date().toISOString().split('T')[0];
  isNavigated: boolean = false;

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDepartments();

    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id && !this.person) {
      this.isNavigated = true;
      this.loadPerson(id);
    } else if (!this.person) {
      this.initializePerson();
    }
  }

  loadPerson(id: number): void {
    this.personService.getById(id).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          alert(`Person with ID ${id} not found. Redirecting to the person list.`);
          this.router.navigate(['/persons']);
        } else {
          alert('An error occurred while fetching person details.');
        }
        return of(null);
      })
    ).subscribe(person => {
      if (person) {
        this.person = person;
      } else {
        this.initializePerson();
      }
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(depts => this.departments = depts);
  }

  initializePerson(): void {
    this.person = {
      id: 0,
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      email: '',
      departmentId: 0,
      departmentName: ""
    };
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
      this.router.navigate(['/persons']);
    } else {
      this.close.emit();
    }
  }
}
