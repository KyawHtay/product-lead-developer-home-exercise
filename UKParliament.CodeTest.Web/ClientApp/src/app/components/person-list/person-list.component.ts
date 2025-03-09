import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})


export class PersonListComponent implements OnInit {
  people: PersonViewModel[] = [];
  constructor(private personService: PersonService, private router: Router) {}
  ngOnInit(): void {
    this.loadPeople();
  }
  loadPeople(): void {
    this.personService.getAll().subscribe(data => this.people = data);
  }
  editPerson(id: number): void {
    console.log(id);
    this.router.navigate(['/edit', id]);
  }
  addPerson(): void {
    this.router.navigate(['/new']);
  }

  

  deletePerson(id: number): void {
    this.personService.delete(id).subscribe(() => this.people = this.people.filter(person => person.id !== id));
  }
}
