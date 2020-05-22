import { Component, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';

declare const amp;

@Component({
    selector: 'patient-video-summary',
    templateUrl: './patient-video-summary.component.html',
    styleUrls: ['./patient-video-summary.component.less']
})
export class PatientVideoSummaryComponent {

    @ViewChild("videoPlayer") videoPlayer: ElementRef;

    @Input() patientVideoDetails: any;

    @Output() onVideoClicked: EventEmitter<any> = new EventEmitter();

    private videoPlayerInstance: any;
    private options = {
        techOrder: ["azureHtml5JS", "flashSS", "html5FairPlayHLS", "silverlightSS", "html5"],
        "nativeControlsForTouch": false,
        autoplay: false,
        controls: false,
        muted: false,
        height: "150",
        poster: ""
    };

    ngAfterViewInit() {
        this.videoPlayerInstance = amp(this.videoPlayer.nativeElement, this.options, () => {
            this.videoPlayerInstance.addEventListener('ended', () => {
                console.log('Finished!');
            });
        });

        this.videoPlayerInstance.src([{
            src: this.patientVideoDetails.MediaServiceUrl,
            type: "application/vnd.ms-sstr+xml"
        }]);
    }

    videoClicked(ev) {
        this.onVideoClicked.emit({
            url: this.patientVideoDetails.MediaServiceUrl
        });
    }

}
