import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMemberFormComponent } from './create-member-form.component';

describe('CreateMemberFormComponent', () => {
  let component: CreateMemberFormComponent;
  let fixture: ComponentFixture<CreateMemberFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateMemberFormComponent]
    });
    fixture = TestBed.createComponent(CreateMemberFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
