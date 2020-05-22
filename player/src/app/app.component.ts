import { Component, ViewChild, ElementRef, ViewChildren, QueryList } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PatientVideoSummaryComponent } from "./patient-video-summary.component";

declare const amp;

interface PatientDetails {
  patientName: string;
  patientId: string;
};

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {

  @ViewChild("videoPlayer") videoPlayer: ElementRef;
  @ViewChildren(PatientVideoSummaryComponent) currPatientVideos: QueryList<PatientVideoSummaryComponent>;

  public selectedPatient: string;
  public markerSkill: string;
  public markerTarget: string;
  public markerResult: string;
  public markerComments: string;
  public isAddingMarker: boolean = false;

  public title = 'player';
  public patientDetails: PatientDetails[];
  public selectedPatientVideos: any;
  public selectedPtntIndx: number = -1;

  public options = {
    techOrder: ["azureHtml5JS", "flashSS", "html5FairPlayHLS", "silverlightSS", "html5"],
    "nativeControlsForTouch": false,
    autoplay: true,
    controls: true,
    muted: false,
    width: "700",
    height: "470",
    poster: ""
  };

  private videoPlayerInstance: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getAllPatients();
  }

  ngAfterViewInit() {
    this.videoPlayerInstance = amp(this.videoPlayer.nativeElement, this.options, () => {
      // this.videoPlayerInstance.addEventListener('sourceset', () => {
      //   console.log("source set!!");
      // });
    });
  }

  addMarkerToVideo(ev) {
    this.videoPlayerInstance.pause();
    this.isAddingMarker = true;
  }

  getAllPatients() {
    this.http.get<PatientDetails[]>("https://fsiautismny2.azurewebsites.net/api/patients", { responseType: "json", observe: "body" })
      .subscribe(res => {
        this.patientDetails = res;
      });
  }

  getPatientVideos(id, idx) {
    this.selectedPatient = id;
    this.selectedPtntIndx = idx;
    this.http.get(`https://fsiautismny2.azurewebsites.net/api/Videos?patientId=${id}`, { responseType: "json", observe: "body" })
      .subscribe(res => {
        this.selectedPatientVideos = res;
      });
  }

  onPatientVideoSelected(info) {
    this.videoPlayerInstance.src([{
      src: info.url,
      type: "application/vnd.ms-sstr+xml"
    }]);
  }

  storeMarker(ev) {

    let timestamp = this.videoPlayerInstance.currentTime();

    var pdata = {
      PatientID: this.selectedPatient,
      TimeStamp: timestamp,
      Skill: this.markerSkill,
      Target: this.markerTarget,
      Result: this.markerResult,
      Comments: this.markerComments
    };

    this.http.post("https://fsiautismny2.azurewebsites.net/api/Data", pdata, {
      headers: {
        "Content-Type": "application/json"
      },
      observe: "response"
    })
      .subscribe(res => {
        console.log(res);
        this.isAddingMarker = false;
      });
  }
}
