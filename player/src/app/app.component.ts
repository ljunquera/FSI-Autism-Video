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

  private markerApi: string = "https://fsiautismny2.azurewebsites.net/api/Marker";
  private eventApi: string = "https://fsiautismny2.azurewebsites.net/api/Data";

  public infoType: string = "";

  public selectedPatient: string;
  public markerSkill: string;
  public markerTarget: string;
  public markerResult: string;
  public markerComments: string;
  public markerTags: string;

  public isAddingMarker: boolean = false;

  public title = 'player';
  public patientDetails: PatientDetails[];
  public selectedPatientVideos: any;
  public selectedPtntIndx: number = -1;
  public selectedMarkerIndx: number = -1;
  public selectedVideUrl: string;

  public availableMarkers: any = [];

  public options = {
    techOrder: ["azureHtml5JS", "flashSS", "html5FairPlayHLS", "silverlightSS", "html5"],
    "nativeControlsForTouch": false,
    autoplay: true,
    controls: true,
    muted: false,
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
    this.infoType = "marker";
  }

  addEventToVideo(ev) {
    this.videoPlayerInstance.pause();
    this.isAddingMarker = true;
    this.infoType = "event";
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
    this.selectedVideUrl = info.url;
    this.videoPlayerInstance.src([{
      src: info.url,
      type: "application/vnd.ms-sstr+xml"
    }]);
    this.getAllMarkersForVideo(info.url);
  }

  storeMarker(ev) {

    let timestamp = this.videoPlayerInstance.currentTime();

    let pData = null;

    if (this.infoType === "marker") {
      pData = {
        PatientID: this.selectedPatient,
        RowKey: new Date().getTime(),
        MarkerTime: timestamp,
        Tag: this.markerTags,
        FileName: this.selectedVideUrl
      };
    }
    else {
      pData = {
        PatientID: this.selectedPatient,
        TimeStamp: timestamp,
        Skill: this.markerSkill,
        Target: this.markerTarget,
        Result: this.markerResult,
        Comments: this.markerComments
      };
    }

    let url = this.infoType === "marker" ? this.markerApi : this.eventApi;

    this.http.post(url, pData, {
      headers: {
        "Content-Type": "application/json"
      },
      observe: "response"
    })
      .subscribe(res => {
        console.log(res);
        this.isAddingMarker = false;
        if(this.infoType === "marker") {
          this.getAllMarkersForVideo(this.selectedVideUrl);
        }
      });
  }

  getAllMarkersForVideo(videoUrl) {
    this.http.get(`${this.markerApi}?patientId=${this.selectedPatient}&fileName=${videoUrl}`, { responseType: "json", observe: "body" })
      .subscribe(res => {
        this.availableMarkers = res;
      });
  }

  seekToCurrentMarker(marker, idx) {
    this.selectedMarkerIndx = idx;
    this.videoPlayerInstance.currentTime(parseFloat(marker.MarkerTime));
    this.videoPlayerInstance.pause();
  }
}
