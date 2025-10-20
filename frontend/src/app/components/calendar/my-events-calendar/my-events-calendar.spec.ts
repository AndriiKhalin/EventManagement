import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyEventsCalendar } from './my-events-calendar';

describe('MyEventsCalendar', () => {
  let component: MyEventsCalendar;
  let fixture: ComponentFixture<MyEventsCalendar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyEventsCalendar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyEventsCalendar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
