﻿using UnityEngine;
using UnityEngine.UI;


namespace UTJ
{
    [ExecuteInEditMode]
    public class MovieRecorderEditorUI : IMovieRecoerderUI
    {
        public IMovieRecorder m_recorder;
        public Text m_textInfo;
        public RawImage m_imagePreview;
        public Slider m_timeSlider;
        public InputField m_inputCurrentFrame;
        public RenderTexture m_rt;
        int m_currentFrame = 0;
        int m_beginFrame = 0;
        int m_endFrame = -1;
        bool m_updateStatus;
        bool m_updatePreview;


        public override bool record
        {
            get { return m_recorder.recording; }
            set
            {
                m_updateStatus = true;
                if (value)
                {
                    m_recorder.BeginRecording();
                    m_recorder.recording = true;
                    GetComponent<Image>().color = new Color(1.0f, 0.5f, 0.5f, 0.5f);
                    UpdatePreviewImage(m_recorder.GetScratchBuffer());
                }
                else
                {
                    m_recorder.recording = false;
                    GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    m_endFrame = -1;
                }
            }
        }

        public override IMovieRecorder GetRecorder()
        {
            return m_recorder;
        }

        public override string GetOutputPath()
        {
            return m_recorder.GetOutputPath();
        }

        public override void Flush()
        {
            m_recorder.Flush(m_beginFrame, m_endFrame);
        }

        public override void Restart()
        {
            m_recorder.EndRecording();
            m_updateStatus = true;
            m_updatePreview = true;
        }


        public int beginFrame
        {
            get { return m_beginFrame; }
            set
            {
                m_beginFrame = value;
                if (m_endFrame >= 0)
                {
                    m_endFrame = Mathf.Max(m_endFrame, m_beginFrame);
                }
                m_updateStatus = true;
            }
        }

        public int endFrame
        {
            get { return m_endFrame; }
            set
            {
                m_endFrame = value;
                m_beginFrame = Mathf.Min(m_endFrame, m_beginFrame);
                m_updateStatus = true;
            }
        }

        public int currentFrame
        {
            get { return m_currentFrame; }
            set
            {
                m_currentFrame = value;
                m_updateStatus = true;
                m_updatePreview = true;
                m_inputCurrentFrame.text = value.ToString();
            }
        }


        public void SetCurrentFrame(float v)
        {
            currentFrame = (int)v;
        }

        public void SetBeginFrame()
        {
            beginFrame = currentFrame;
        }

        public void SetEndFrame()
        {
            endFrame = currentFrame;
        }

        public void EraseFrames()
        {
            if (m_endFrame >= 0)
            {
                m_recorder.EraseFrame(m_beginFrame, m_endFrame);
                m_beginFrame = 0;
                m_endFrame = -1;
                m_updateStatus = true;
            }
        }


        void UpdatePreviewImage(RenderTexture rt)
        {
            const float MaxXScale = 1.8f;
            m_imagePreview.texture = rt;
            float s = (float)rt.width / (float)rt.height;
            float xs = Mathf.Min(s, MaxXScale);
            float ys = MaxXScale / s;
            m_imagePreview.rectTransform.localScale = new Vector3(xs, ys, 1.0f);
        }



        void OnEnable()
        {
#if UNITY_EDITOR
            if (m_recorder == null)
            {
                m_recorder = FindObjectOfType<IMovieRecorder>();
            }
#endif // UNITY_EDITOR
        }

        void OnDisable()
        {
            if (m_rt != null)
            {
                m_rt.Release();
                m_rt = null;
            }
        }

        void Update()
        {
            if (m_rt == null)
            {
                var buf = m_recorder.GetScratchBuffer();
                if (buf != null)
                {
                    m_rt = new RenderTexture(buf.width, buf.height, 0, RenderTextureFormat.ARGB32);
                    m_rt.wrapMode = TextureWrapMode.Repeat;
                    m_rt.Create();
                }
            }
            if (m_updatePreview)
            {
                m_updatePreview = false;
                m_recorder.GetFrameData(m_rt, m_currentFrame);
                UpdatePreviewImage(m_rt);
            }
            if (m_updateStatus || record)
            {
                m_updateStatus = false;
                int recoded_frames = m_recorder.GetFrameCount();
                m_timeSlider.maxValue = recoded_frames;
                int begin_frame = m_beginFrame;
                int end_frame = m_endFrame == -1 ? recoded_frames : m_endFrame;
                int frame_count = end_frame - begin_frame;
                m_textInfo.text =
                    recoded_frames.ToString() + " recoded frames\n" +
                    frame_count.ToString() + " output frames (" + begin_frame + " - " + end_frame + ")\n" +
                    "expected file size: " + m_recorder.GetExpectedFileSize(begin_frame, end_frame);
            }
        }
    }

}
