import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddChild } from './add-child';

describe('AddChild', () => {
  let component: AddChild;
  let fixture: ComponentFixture<AddChild>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddChild]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddChild);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
