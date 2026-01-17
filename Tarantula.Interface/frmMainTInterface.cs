using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tarantula.Indexer;
using Tarantula.Crawler;

namespace Tarantula.Interface
{
    public partial class frmMainTInterface : Form
    {
        TCrawler crawler = new TCrawler(maxPages: 500);
        TIndexer indexer = new TIndexer();
        public frmMainTInterface()
        {
            InitializeComponent();
        }
    }
}
