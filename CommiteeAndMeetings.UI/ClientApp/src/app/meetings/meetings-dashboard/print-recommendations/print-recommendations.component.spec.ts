import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintRecommendationsComponent } from './print-recommendations.component';

describe('PrintRecommendationsComponent', () => {
  let component: PrintRecommendationsComponent;
  let fixture: ComponentFixture<PrintRecommendationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrintRecommendationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintRecommendationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
