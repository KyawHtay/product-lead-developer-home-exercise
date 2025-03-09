import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit {
  people: PersonViewModel[] = [];
  selectedPerson: PersonViewModel | null = null;

  constructor(
    private personService: PersonService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.personService.getAll().subscribe(data => {
      this.people = data;
    });
  }

  addPerson(): void {
    this.selectedPerson = {
      id: 0,
      firstName: '',
      lastName: '',
      email: '',
      dateOfBirth: '',
      departmentId: 0,
      department: undefined
    };
  }

  selectPerson(person: PersonViewModel): void {
    this.personService.getById(person.id).subscribe(data => {
      this.selectedPerson = data;
      this.cdr.detectChanges(); 
    });
  }

  deletePerson(id: number): void {
    if (confirm('Are you sure you want to delete this person?')) {
      this.personService.delete(id).subscribe(() => {
        this.people = this.people.filter(person => person.id !== id);
        this.selectedPerson = null;
      });
    }
  }

  closeEditor(): void {
    this.selectedPerson = null;
    this.loadPeople();
  }
}
