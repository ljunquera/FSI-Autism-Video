#include <iostream>

#include <k4a/k4a.h>
#include <k4arecord/playback.hpp>
#include <k4abt.hpp>

//static std::string KinectRecording("C:\\Users\\Jonathan Charest\\Documents\\Sample Kinect Files\\sample.mkv");
static std::string KinectRecording("C:\\Users\\Jonathan Charest\\Documents\\Sample Kinect Files\\sample2.mkv");
//static std::string KinectRecording("C:\\Users\\Jonathan Charest\\Documents\\Sample Kinect Files\\sample3.mkv");

// Helper function to process data from the capture to get body motion.
template <typename ProcessFrame>
void processKinectCapture(k4a::playback& playback, const ProcessFrame& processFrame)
{
	// Create body tracker.
	auto tracker = k4abt::tracker::create(playback.get_calibration());

	k4a::capture cap;
	k4abt::frame frame;
	size_t numCaptures = 0;
	while (playback.get_next_capture(&cap))
	{
		tracker.enqueue_capture(cap);
		tracker.pop_result(&frame);
		processFrame(frame);
	}
}

int main(int agrc, char* argv[])
{
	// Load from file. We will need to see if it can load from Azure storage. API seems limited so we may have to download to temp file...
	auto playback = k4a::playback::open(KinectRecording.c_str());

	size_t numCaptures = 0;
	processKinectCapture(playback, 
		[&](k4abt::frame& frame) 
		{ 
			if (frame.get_num_bodies() > 0)
			{
				std::cout << "Found body on frame " << numCaptures << std::endl;
			}
			++numCaptures;  
		});


	std::cout << "Processed " << numCaptures << " captures" << std::endl;
}

