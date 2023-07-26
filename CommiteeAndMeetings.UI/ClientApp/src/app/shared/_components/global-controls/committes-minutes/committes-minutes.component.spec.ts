import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommittesMinutesComponent } from './committes-minutes.component';

describe('CommittesMinutesComponent', () => {
  let component: CommittesMinutesComponent;
  let fixture: ComponentFixture<CommittesMinutesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommittesMinutesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommittesMinutesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
