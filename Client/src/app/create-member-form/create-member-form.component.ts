import { Component, Output, EventEmitter } from '@angular/core';
import { Member } from '../models';

@Component({
  selector: 'app-create-member-form',
  templateUrl: './create-member-form.component.html',
  styleUrls: ['./create-member-form.component.scss']
})
export class CreateMemberFormComponent {
  @Output() memberCreated = new EventEmitter<Member>();

  newMember: Member = {
    name: '',
    lastName: '',
    identityNumber: '',
    city: '',
    street: '',
    homeNumber: '',
    birthdate: new Date(),
    telephone: '',
    mobilePhone: '',
    isImmune: false,
    immunes: [],
    positiveDate: undefined,
    negativeDate: undefined,
  };

  createMember() {
    this.memberCreated.emit(this.newMember);
    // Optionally, reset form fields here
    this.newMember = {
      name: '',
      lastName: '',
      identityNumber: '',
      city: '',
      street: '',
      homeNumber: '',
      birthdate: new Date(),
      telephone: '',
      mobilePhone: '',
      isImmune: false,
      immunes: [],
      positiveDate: undefined,
      negativeDate: undefined,
    }
  }
}
