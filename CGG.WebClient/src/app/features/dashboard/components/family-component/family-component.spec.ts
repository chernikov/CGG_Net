import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FamilyComponent } from './family-component';

describe('FamilyComponent', () => {
  let component: FamilyComponent;
  let fixture: ComponentFixture<FamilyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FamilyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FamilyComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
