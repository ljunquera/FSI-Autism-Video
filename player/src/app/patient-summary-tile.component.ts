import { Component, ViewChild, ElementRef, Input } from '@angular/core';

@Component({
  selector: 'patient-summary-tile',
  templateUrl: './patient-summary-tile.component.html',
  styleUrls: ['./patient-summary-tile.component.less']
})
export class PatientSummaryTileComponent {

    @Input() patientDetails: any;
    
}
