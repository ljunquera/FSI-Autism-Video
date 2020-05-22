import { Component, ViewChild, ElementRef } from '@angular/core';

declare const amp;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {

  @ViewChild("videoPlayer") videoPlayer: ElementRef;

  title = 'player';

  private options = {
    techOrder: ["azureHtml5JS", "flashSS", "html5FairPlayHLS", "silverlightSS", "html5"],
    "nativeControlsForTouch": false,
    autoplay: false,
    controls: true,
    width: "640",
    height: "400",
    poster: ""
  };

  private videoPlayerInstance: any;

  ngAfterViewInit() {
    this.videoPlayerInstance = amp(this.videoPlayer.nativeElement, this.options, () => {
      console.log('Good to go!');
      let _this: any = this;
      // add an event listener
      _this.addEventListener('ended', () => {
        console.log('Finished!');
      });
    });

    this.videoPlayerInstance.src([{
      src: "https://autismhackathoncase2-usea.streaming.media.azure.net/a9ce346f-43d3-4602-8e7a-3cbb7950ffc1/ignite.ism/manifest",
      type: "application/vnd.ms-sstr+xml"
    }]);    
  }

  seekVideo(ev) {
    this.videoPlayerInstance.currentTime(60);
  }

  playVideo(ev) {
    this.videoPlayerInstance.play();
  }

  pauseVideo(ev) {
    this.videoPlayerInstance.pause();
  }
}
