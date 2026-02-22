import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditChild } from './edit-child';

describe('EditChild', () => {
  let component: EditChild;
  let fixture: ComponentFixture<EditChild>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditChild]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditChild);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
