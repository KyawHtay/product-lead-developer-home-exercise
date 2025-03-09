import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(private personService: PersonService, private router: Router) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.personService.getAll().subscribe(data => this.people = data);
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
    }; // Empty person object for adding
  }

  selectPerson(person: PersonViewModel): void {
    this.selectedPerson = { ...person }; // Clone object for editing
  }

  deletePerson(id: number): void {
    if (confirm('Are you sure you want to delete this person?')) {
      this.personService.delete(id).subscribe(() => {
        this.people = this.people.filter(person => person.id !== id);
        this.selectedPerson = null; // Close editor if person was selected
      });
    }
  }

  closeEditor(): void {
    this.selectedPerson = null; // Hide editor
    this.loadPeople(); // Reload updated list
  }
}
